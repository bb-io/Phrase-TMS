using Apps.PhraseTMS.Extensions;
using Apps.PhraseTMS.Helpers;
using Apps.PhraseTMS.Models.InteroperableXliff;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Extensions;
using Blackbird.Filters.Transformations;
using RestSharp;
using System.Text;

namespace Apps.PhraseTMS.Actions;

[ActionList("Interoperable XLIFF (experimental)")]
public class InteroperableXliffActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : PhraseInvocable(invocationContext)
{
    [Action("Download interoperable XLIFF (experimental)", Description = "Export XLIFF file with normalized metadata and content, so it could be reused in all other actions.")]
    public async Task<DownloadInteroperableXliffResponse> DownloadInteroperableXliff(
        [ActionParameter] ProjectRequest projectInput,
        [ActionParameter] JobRequest jobInput)
    {
        var requestFile = new RestRequest($"/api2/v1/projects/{projectInput.ProjectUId}/jobs/bilingualFile", Method.Post)
            .WithJsonBody(new
            {
                jobs = new[]
                {
                    new { uid = jobInput.JobUId }
                },
            });

        var responseFile = await Client.ExecuteWithHandling(requestFile);

        var fileData = responseFile.RawBytes;
        var fileName = responseFile.GetFilenameFromHeader("default_target_file");

        if ((fileData?.LongLength ?? 0) == 0)
            throw new PluginApplicationException("Empty file received from Phrase TMS.");

        var fileString = Encoding.UTF8.GetString(fileData ?? []);
        var transformation = Transformation.Parse(fileString, fileName);

        var response = new DownloadInteroperableXliffResponse()
        {
            SourceLocale = transformation.SourceLanguage ?? string.Empty,
            TargetLocale = transformation.TargetLanguage ?? string.Empty
        };

        foreach (var unit in transformation.GetUnits())
        {
            response.TotalSegments += unit.Segments.Count;

            // Normalize segment states
            var (targetState, updateCount) = unit switch
            {
                _ when MXLIFFHelper.IsLocked(unit) => (SegmentState.Final, (Action<DownloadInteroperableXliffResponse>)(r => r.LockedSegments++)),
                _ when MXLIFFHelper.IsConfirmed(unit) => (SegmentState.Reviewed, (Action<DownloadInteroperableXliffResponse>)(r => r.ConfirmedSegments++)),
                _ when unit.Segments.Any(s => !string.IsNullOrEmpty(s.GetTarget())) => (SegmentState.Translated, null),
                _ => (SegmentState.Initial, (Action<DownloadInteroperableXliffResponse>)(r => r.EmptyTargetSegments++))
            };

            foreach (var segment in unit.Segments)
            {
                segment.State = targetState;
                updateCount?.Invoke(response);
            }

            // Normalize character limits
            if (MXLIFFHelper.GetMaxLen(unit, transformation) is int characterLimit)
                unit.SizeRestrictions.MaximumSize = characterLimit;

            // Normalize scores
            if (double.TryParse(MXLIFFHelper.GetQeScore(unit), out var qeScore))
            {
                unit.Quality.Score = qeScore;
                unit.Quality.ProfileReference = "QE by Phrase TMS";
            }

            // Normalize metadata
            if (MXLIFFHelper.GetOrigin(unit) is string origin)
                unit.Provenance.Translation.Tool = $"Phrase TMS ({origin})";
        }

        response.File = await fileManagementClient.UploadAsync(
            transformation.Serialize().ToStream(),
            "application/xliff+xml",
            fileName.Replace(".mxliff", ".xliff"));

        return response;
    }
}
