namespace EducationPortal.Domain.Core.Entities
{
    using System.Collections.Generic;
    using EducationPortal.Domain.Core.Mappings;

    public class Author : BaseEntity
    {
        public Author()
        {
            this.AuthorPrintedMaterials = new List<AuthorPrintedMaterial>();
        }

        public string Name { get; set; }

        public List<AuthorPrintedMaterial> AuthorPrintedMaterials { get; set; }
    }
}
