﻿namespace Apps.PhraseTms.Models.Jobs.Responses
{
    public class GetJobResponse
    {
        public string Filename { get; set; }

        public string Status { get; set; }

        public string TargetLanguage { get; set; }

        public string DateDue { get; set; }
    }
}
