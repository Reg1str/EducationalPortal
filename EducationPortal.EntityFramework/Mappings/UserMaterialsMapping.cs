using EducationPortal.Domain.Core.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EducationPortal.EntityFramework.Mappings
{
    public class UserMaterialsMapping : IEntityTypeConfiguration<UserMaterials>
    {
        public void Configure(EntityTypeBuilder<UserMaterials> builder)
        {
            builder.HasKey(userSkill => new { userSkill.MaterialId, userSkill.UserId });

            builder.Property(userSkill => userSkill.IsFinished).IsRequired();

            builder.HasOne(userMaterial => userMaterial.User)
                   .WithMany(user => user.UserMaterials)
                   .HasForeignKey(userMaterial => userMaterial.UserId)
                   .HasPrincipalKey(user => user.Id);

            builder.HasOne(userMaterial => userMaterial.Material)
                   .WithMany(material => material.UserMaterials)
                   .HasForeignKey(userMaterial => userMaterial.MaterialId)
                   .HasPrincipalKey(material => material.Id);
        }
    }
}
