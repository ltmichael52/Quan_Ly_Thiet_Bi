using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ThietBiDienTu_2.Models;

public partial class ToolDbContext : DbContext
{
    public ToolDbContext()
    {
    }

    public ToolDbContext(DbContextOptions<ToolDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Chitietphieumuon> Chitietphieumuons { get; set; }

    public virtual DbSet<Chitietthietbi> Chitietthietbis { get; set; }

    public virtual DbSet<Coso> Cosos { get; set; }

    public virtual DbSet<Loaithietbi> Loaithietbis { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Phieumuon> Phieumuons { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<Sinhvien> Sinhviens { get; set; }

    public virtual DbSet<Thietbi> Thietbis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MICHAEL;Initial Catalog=QLTHIETBI;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK__ACCOUNT__6029121DD1D4BE32");

            entity.ToTable("ACCOUNT");

            entity.Property(e => e.Username)
                .ValueGeneratedNever()
                .HasColumnName("USERNAME");
            entity.Property(e => e.Loaiuser).HasColumnName("LOAIUSER");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("PASSWORD");
        });

        modelBuilder.Entity<Chitietphieumuon>(entity =>
        {
            entity.HasKey(e => new { e.Mapm, e.Seri });

            entity.ToTable("CHITIETPHIEUMUON");

            entity.Property(e => e.Mapm).HasColumnName("MAPM");
            entity.Property(e => e.Seri)
                .HasMaxLength(255)
                .HasColumnName("SERI");
            entity.Property(e => e.Ngaytra)
                .HasColumnType("datetime")
                .HasColumnName("NGAYTRA");

            entity.HasOne(d => d.MapmNavigation).WithMany(p => p.Chitietphieumuons)
                .HasForeignKey(d => d.Mapm)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETPHIEUMUON_PHIEUMUON1");

            entity.HasOne(d => d.SeriNavigation).WithMany(p => p.Chitietphieumuons)
                .HasForeignKey(d => d.Seri)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETPHIEUMUON_CHITIETTHIETBI");
        });

        modelBuilder.Entity<Chitietthietbi>(entity =>
        {
            entity.HasKey(e => e.Seri);

            entity.ToTable("CHITIETTHIETBI");

            entity.Property(e => e.Seri)
                .HasMaxLength(255)
                .HasColumnName("SERI");
            entity.Property(e => e.Map)
                .HasMaxLength(30)
                .HasColumnName("MAP");
            entity.Property(e => e.Matb)
                .HasMaxLength(255)
                .HasColumnName("MATB");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(255)
                .HasColumnName("TRANGTHAI");

            entity.HasOne(d => d.MapNavigation).WithMany(p => p.Chitietthietbis)
                .HasForeignKey(d => d.Map)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETTHIETBI_PHONG");

            entity.HasOne(d => d.MatbNavigation).WithMany(p => p.Chitietthietbis)
                .HasForeignKey(d => d.Matb)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CHITIETTHIETBI_THIETBI");
        });

        modelBuilder.Entity<Coso>(entity =>
        {
            entity.HasKey(e => e.Macs).HasName("PK__COSO__603F183B80D63A70");

            entity.ToTable("COSO");

            entity.Property(e => e.Macs).HasColumnName("MACS");
            entity.Property(e => e.Diachi)
                .HasMaxLength(255)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Tencs)
                .HasMaxLength(255)
                .HasColumnName("TENCS");
        });

        modelBuilder.Entity<Loaithietbi>(entity =>
        {
            entity.HasKey(e => e.Maloai);

            entity.ToTable("LOAITHIETBI");

            entity.Property(e => e.Maloai)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MALOAI");
            entity.Property(e => e.Tenloai)
                .HasMaxLength(50)
                .HasColumnName("TENLOAI");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.Manv).HasName("PK__NHANVIEN__603F511441B78137");

            entity.ToTable("NHANVIEN");

            entity.Property(e => e.Manv)
                .ValueGeneratedNever()
                .HasColumnName("MANV");
            entity.Property(e => e.Diachi)
                .HasMaxLength(255)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Gioitinh)
                .HasMaxLength(3)
                .HasColumnName("GIOITINH");
            entity.Property(e => e.Ngaysinh)
                .HasColumnType("datetime")
                .HasColumnName("NGAYSINH");
            entity.Property(e => e.Sdt)
                .HasMaxLength(255)
                .HasColumnName("SDT");
            entity.Property(e => e.Tennv)
                .HasMaxLength(255)
                .HasColumnName("TENNV");

            entity.HasOne(d => d.ManvNavigation).WithOne(p => p.Nhanvien)
                .HasForeignKey<Nhanvien>(d => d.Manv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ACCOUNT__NV");
        });

        modelBuilder.Entity<Phieumuon>(entity =>
        {
            entity.HasKey(e => e.Mapm).HasName("PK__PHIEUMUO__603F61CDEBB7FD91");

            entity.ToTable("PHIEUMUON");

            entity.Property(e => e.Mapm).HasColumnName("MAPM");
            entity.Property(e => e.Manv).HasColumnName("MANV");
            entity.Property(e => e.Masv).HasColumnName("MASV");
            entity.Property(e => e.Ngaylap)
                .HasColumnType("datetime")
                .HasColumnName("NGAYLAP");
            entity.Property(e => e.Ngaymuon)
                .HasColumnType("datetime")
                .HasColumnName("NGAYMUON");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(255)
                .HasColumnName("TRANGTHAI");

            entity.HasOne(d => d.ManvNavigation).WithMany(p => p.Phieumuons)
                .HasForeignKey(d => d.Manv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PHIEUMUON_NHANVIEN1");

            entity.HasOne(d => d.MasvNavigation).WithMany(p => p.Phieumuons)
                .HasForeignKey(d => d.Masv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PHIEUMUON_SINHVIEN");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.Map).HasName("PK__PHONG__C790778043DD78DD");

            entity.ToTable("PHONG");

            entity.Property(e => e.Map)
                .HasMaxLength(30)
                .HasColumnName("MAP");
            entity.Property(e => e.Loaiphong)
                .HasMaxLength(255)
                .HasColumnName("LOAIPHONG");
            entity.Property(e => e.Macs).HasColumnName("MACS");
            entity.Property(e => e.Tenphong)
                .HasMaxLength(255)
                .HasColumnName("TENPHONG");

            entity.HasOne(d => d.MacsNavigation).WithMany(p => p.Phongs)
                .HasForeignKey(d => d.Macs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PHONG__MACS__5165187F");
        });

        modelBuilder.Entity<Sinhvien>(entity =>
        {
            entity.HasKey(e => e.Masv).HasName("PK__SINHVIEN__60228A2812E98CDF");

            entity.ToTable("SINHVIEN");

            entity.Property(e => e.Masv)
                .ValueGeneratedNever()
                .HasColumnName("MASV");
            entity.Property(e => e.Diachi)
                .HasMaxLength(255)
                .HasColumnName("DIACHI");
            entity.Property(e => e.Diemrenluyen).HasColumnName("DIEMRENLUYEN");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Gioitinh)
                .HasMaxLength(3)
                .HasDefaultValueSql("(N'')")
                .HasColumnName("GIOITINH");
            entity.Property(e => e.Khoa)
                .HasMaxLength(255)
                .HasColumnName("KHOA");
            entity.Property(e => e.Nganh)
                .HasMaxLength(255)
                .HasColumnName("NGANH");
            entity.Property(e => e.Ngaysinh)
                .HasColumnType("datetime")
                .HasColumnName("NGAYSINH");
            entity.Property(e => e.Sdt)
                .HasMaxLength(255)
                .HasColumnName("SDT");
            entity.Property(e => e.Tensv)
                .HasMaxLength(255)
                .HasColumnName("TENSV");

            entity.HasOne(d => d.MasvNavigation).WithOne(p => p.Sinhvien)
                .HasForeignKey<Sinhvien>(d => d.Masv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ACCOUNT__SV");
        });

        modelBuilder.Entity<Thietbi>(entity =>
        {
            entity.HasKey(e => e.Matb).HasName("PK__THIETBI__6023721DFE47CEB0");

            entity.ToTable("THIETBI");

            entity.Property(e => e.Matb)
                .HasMaxLength(255)
                .HasColumnName("MATB");
            entity.Property(e => e.Hinhanh).HasColumnName("HINHANH");
            entity.Property(e => e.Maloai)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MALOAI");
            entity.Property(e => e.Mota).HasColumnName("MOTA");
            entity.Property(e => e.Soluong).HasColumnName("SOLUONG");
            entity.Property(e => e.Tenthietbi)
                .HasMaxLength(255)
                .HasColumnName("TENTHIETBI");

            entity.HasOne(d => d.MaloaiNavigation).WithMany(p => p.Thietbis)
                .HasForeignKey(d => d.Maloai)
                .HasConstraintName("FK_THIETBI_LOAITHIETBI");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
