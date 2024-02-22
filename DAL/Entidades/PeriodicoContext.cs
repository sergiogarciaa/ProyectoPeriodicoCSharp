using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entidades;

public partial class PeriodicoContext : DbContext
{
    public PeriodicoContext()
    {
    }

    public PeriodicoContext(DbContextOptions<PeriodicoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Noticia> Noticias { get; set; }

    public virtual DbSet<NoticiaComentario> NoticiaComentarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioComentario> UsuarioComentarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=periodicoC;UserId=postgres;Password=");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("categorias_pkey");

            entity.ToTable("categorias", "prdc_schema");

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.DescCategoria)
                .HasMaxLength(255)
                .HasColumnName("desc_categoria");
            entity.Property(e => e.TipoCategoria)
                .HasMaxLength(255)
                .HasColumnName("tipo_categoria");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdComentario).HasName("comentarios_pkey");
            entity.ToTable("comentarios", "prdc_schema");

            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.IdNoticiaNavigation) // Un comentario pertenece a una noticia
                .WithMany(n => n.Comentarios) // Una noticia puede tener varios comentarios
                .HasForeignKey(c => c.Id_Noticia) // La clave foránea en Comentario
                .OnDelete(DeleteBehavior.Cascade); // Opcional: establecer la acción de eliminación en caso de que se elimine una noticia
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Usuario) // Un comentario pertenece a un usuario
                .WithMany(u => u.Comentarios) // Un usuario puede tener varios comentarios
                .HasForeignKey(c => c.IdUsuario) // La clave foránea en Comentario
                .OnDelete(DeleteBehavior.ClientSetNull); // Opcional: establecer la acción de eliminación en caso de que se elimine un usuario

            entity.Property(e => e.IdComentario).HasColumnName("id_comentario");
            entity.Property(e => e.DescComentario)
                .HasMaxLength(1000)
                .HasColumnName("desc_comentario");
            entity.Property(e => e.FchPublicacionComentario)
                .HasMaxLength(255)
                .HasColumnName("fch_publicacion_comentario");
        });

        modelBuilder.Entity<Noticia>(entity =>
        {
            entity.HasKey(e => e.IdNoticia).HasName("noticias_pkey");

            entity.ToTable("noticias", "prdc_schema");

            // Configuración de la relación con Comentario
            entity.HasMany(n => n.Comentarios)
                  .WithOne(c => c.IdNoticiaNavigation)
                  .HasForeignKey(c => c.Id_Noticia)
                  .HasConstraintName("fk_comentario_noticia");

            entity.Property(e => e.IdNoticia).HasColumnName("id_noticia");
            entity.Property(e => e.DescNoticia)
                .HasMaxLength(4000)
                .HasColumnName("desc_noticia");
            entity.Property(e => e.FchPublicacion)
                .HasMaxLength(255)
                .HasColumnName("fch_publicacion");
            entity.Property(e => e.IdCategoriaNoticia).HasColumnName("id_categoria_noticia");
            entity.Property(e => e.IdUsuarioNoticia).HasColumnName("id_usuario_noticia");
            entity.Property(e => e.ImagenNoticia).HasColumnName("imagen_noticia");
            entity.Property(e => e.RequiereSuscripcion).HasColumnName("requiere_suscripcion");
            entity.Property(e => e.TituloNoticia)
                .HasMaxLength(200)
                .HasColumnName("titulo_noticia");

            entity.HasOne(d => d.IdCategoriaNoticiaNavigation).WithMany(p => p.Noticia)
                .HasForeignKey(d => d.IdCategoriaNoticia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkab91df7dfpltux7gcrqjpru34");

            entity.HasOne(d => d.IdUsuarioNoticiaNavigation).WithMany(p => p.Noticia)
                .HasForeignKey(d => d.IdUsuarioNoticia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkrbdei6nfn304qyyonge87ualu");
        });

        modelBuilder.Entity<NoticiaComentario>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("noticia_comentarios", "prdc_schema");

            entity.Property(e => e.IdComentario).HasColumnName("id_comentario");
            entity.Property(e => e.IdNoticia).HasColumnName("id_noticia");

            entity.HasOne(d => d.IdComentarioNavigation).WithMany()
                .HasForeignKey(d => d.IdComentario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkf12y8y8spo9x27n69mvxjk9go");

            entity.HasOne(d => d.IdNoticiaNavigation).WithMany()
                .HasForeignKey(d => d.IdNoticia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkqd9aagyg91260vcyh5aaph6xp");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuarios_pkey");

            entity.ToTable("usuarios", "prdc_schema_usuarios");

            entity.HasIndex(e => e.DniUsuario, "usuarios_dni_usuario_key").IsUnique();

            entity.HasIndex(e => e.EmailUsuario, "usuarios_email_usuario_key").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.ApellidosUsuario)
                .HasMaxLength(100)
                .HasColumnName("apellidos_usuario");
            entity.Property(e => e.ClaveUsuario)
                .HasMaxLength(100)
                .HasColumnName("clave_usuario");
            entity.Property(e => e.CuentaConfirmada)
                .HasDefaultValue(false)
                .HasColumnName("cuenta_confirmada");
            entity.Property(e => e.DniUsuario)
                .HasMaxLength(9)
                .HasColumnName("dni_usuario");
            entity.Property(e => e.EmailUsuario)
                .HasMaxLength(100)
                .HasColumnName("email_usuario");
            entity.Property(e => e.EstadoSuscripcion).HasColumnName("estado_suscripcion");
            entity.Property(e => e.ExpiracionToken)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("expiracion_token");
            entity.Property(e => e.FchAltaUsuario)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("fch_alta_usuario");
            entity.Property(e => e.FchBajaUsuario)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("fch_baja_usuario");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(70)
                .HasColumnName("nombre_usuario");
            entity.Property(e => e.Rol)
                .HasMaxLength(255)
                .HasColumnName("rol");
            entity.Property(e => e.TlfUsuario)
                .HasMaxLength(9)
                .HasColumnName("tlf_usuario");
            entity.Property(e => e.TokenRecuperacion)
                .HasMaxLength(100)
                .HasColumnName("token_recuperacion");
        });

        modelBuilder.Entity<UsuarioComentario>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("usuario_comentarios", "prdc_schema");

            entity.Property(e => e.IdComentario).HasColumnName("id_comentario");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdComentarioNavigation).WithMany()
                .HasForeignKey(d => d.IdComentario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fks60gdyncnkxtpby9t6q69cfva");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany()
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkejjm7tjlrrwb2ls7wa4awngf4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
