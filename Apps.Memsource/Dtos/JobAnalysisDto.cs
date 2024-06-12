using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos;

public class JobAnalysisDto
{

    [JsonProperty("uid")]
    public string UId { get; set; }

    [JsonProperty("filename")]
    public string FileName { get; set; }

    public Data data { get; set; }
}
public class All
{
    public double words { get; set; }
}

public class Data
{
    public All all { get; set; }
    public Repetitions repetitions { get; set; }
    public TransMemoryMatches transMemoryMatches { get; set; }
    public MachineTranslationMatches machineTranslationMatches { get; set; }
    public NonTranslatablesMatches nonTranslatablesMatches { get; set; }
    public InternalFuzzyMatches internalFuzzyMatches { get; set; }
}

public class InternalFuzzyMatches
{
    public Match100 match100 { get; set; }
    public Match95 match95 { get; set; }
    public Match85 match85 { get; set; }
    public Match75 match75 { get; set; }
    public Match50 match50 { get; set; }
    public Match0 match0 { get; set; }
}

public class MachineTranslationMatches
{
    public Match100 match100 { get; set; }
    public Match95 match95 { get; set; }
    public Match85 match85 { get; set; }
    public Match75 match75 { get; set; }
    public Match50 match50 { get; set; }
    public Match0 match0 { get; set; }
}

public class Match0
{
    public double words { get; set; }
}

public class Match100
{
    public double words { get; set; }
}

public class Match101
{
    public double words { get; set; }
}

public class Match50
{
    public double words { get; set; }
}

public class Match75
{
     public double words { get; set; }
}

public class Match85
{
    public double words { get; set; }
}

public class Match95
{
    public double words { get; set; }
}

public class Match99
{
    public double words { get; set; }
}

public class NonTranslatablesMatches
{
    public Match100 match100 { get; set; }
    public Match99 match99 { get; set; }
}

public class Repetitions
{
    public double words { get; set; }
}

public class TransMemoryMatches
{
    public Match100 match100 { get; set; }
    public Match95 match95 { get; set; }
    public Match85 match85 { get; set; }
    public Match75 match75 { get; set; }
    public Match50 match50 { get; set; }
    public Match0 match0 { get; set; }
    public Match101 match101 { get; set; }
}