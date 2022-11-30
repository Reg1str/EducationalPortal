namespace EducationPortal.Domain.Core.Mappings
{
    using EducationPortal.Domain.Core.Entities;

    public class AuthorPrintedMaterial
    {
        public int AuthorId { get; set; }

        public Author Author { get; set; }

        public int PrintedMaterialId { get; set; }

        public PrintedMaterial PrintedMaterial { get; set; }
    }
}
