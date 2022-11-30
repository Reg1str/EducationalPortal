namespace EducationPortal.Domain.Core.Entities
{
    using System.Collections.Generic;
    using EducationPortal.Domain.Core.Mappings;

    public class Material : BaseEntity
    {
        public Material()
        {
            this.CourseMaterials = new List<CourseMaterial>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public List<CourseMaterial> CourseMaterials { get; set; }

        public List<UserMaterials> UserMaterials { get; set; }
    }
}
