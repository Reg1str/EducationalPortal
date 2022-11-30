namespace EducationPortal.Domain.Core.Entities
{
    using System.Collections.Generic;
    using EducationPortal.Domain.Core.Mappings;

    public class PrintedMaterial : Material
    {
        public PrintedMaterial()
        {
            this.AuthorPrintedMaterials = new List<AuthorPrintedMaterial>();
        }

        public int PagesCount { get; set; }

        public List<AuthorPrintedMaterial> AuthorPrintedMaterials { get; set; }
    }
}
