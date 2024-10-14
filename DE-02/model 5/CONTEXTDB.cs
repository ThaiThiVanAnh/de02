using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DE_02.model_5
{
    public partial class CONTEXTDB : DbContext
    {
        public CONTEXTDB()
            : base("name=CONTEXTDB")
        {
        }

        public virtual DbSet<LOAISP> LOAISPs { get; set; }
        public virtual DbSet<SANPHAM> SANPHAMs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LOAISP>()
                .Property(e => e.MaLoai)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.MaSP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.MaLoai)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
