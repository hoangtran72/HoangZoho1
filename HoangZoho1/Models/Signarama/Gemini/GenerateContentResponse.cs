using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoangZoho1.Models.Signarama.Gemini
{

    public class GenerateContentResponse
    {

        public GenerateContentResponse()
        {

            candidates = new List<Candidate>();

        }

        public List<Candidate> candidates { get; set; }

        public UsageMetadata usageMetadata { get; set; }

    }

    public class UsageMetadata
    {

        public int? promptTokenCount { get; set; }

        public int? candidatesTokenCount { get; set; }

        public int? totalTokenCount { get; set; }

    }

    public class Candidate
    {

        public Candidate()
        {

            safetyRatings = new List<SafetyRating>();

        }

        public ContentResponse content { get; set; }

        public string finishReason { get; set; }

        public int? index { get; set; }

        public List<SafetyRating> safetyRatings { get; set; }

    }

    public class ContentResponse
    {

        public PartResponse[] parts { get; set; }

        public string role { get; set; }

    }

    public class PartResponse
    {

        public string text { get; set; }

    }

    public class SafetyRating
    {

        public string category { get; set; }

        public string probability { get; set; }

    }

}
