namespace EducationPortal.Domain.Core.Entities
{
    using System;

    public class Article : Material
    {
        public DateTime PublishedDate { get; set; }

        public string SourceUrl { get; set; }
    }
}
