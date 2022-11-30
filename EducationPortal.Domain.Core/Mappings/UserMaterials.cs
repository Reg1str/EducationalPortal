using EducationPortal.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.Domain.Core.Mappings
{
    public class UserMaterials
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int MaterialId { get; set; }

        public Material Material { get; set; }

        public bool IsFinished { get; set; }
    }
}
