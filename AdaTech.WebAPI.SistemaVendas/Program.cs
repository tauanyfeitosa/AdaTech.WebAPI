
using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services;
using Microsoft.EntityFrameworkCore;

namespace AdaTech.WebAPI.SistemaVendas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<EnderecoService>();
            builder.Services.AddScoped<IRepository<Cliente>, ClienteRepository>();
            builder.Services.AddScoped<IRepository<ItemVenda>, ItemVendaRepository>();
            builder.Services.AddScoped<IRepository<DevolucaoTroca>, DevolucaoTrocaRepository>();
            builder.Services.AddScoped<IRepository<Venda>, VendaRepository>();
            builder.Services.AddScoped<IRepository<Produto>, ProdutoRepository>();
            builder.Services.AddScoped<IRepository<Endereco>, EnderecoRepository>();

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlite(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost5500",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5500")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            builder.Services.AddHttpClient();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowLocalhost5500");

            app.MapControllers();

            app.Run();
        }
    }

}