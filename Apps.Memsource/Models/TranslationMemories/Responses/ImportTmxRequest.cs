﻿namespace Apps.PhraseTMS.Models.TranslationMemories.Responses
{
    public class ImportTmxRequest
    {
        public string TranslationMemoryUId { get; set; }

        public byte[] File { get; set; }

        public string Filename { get; set; }
    }
}
