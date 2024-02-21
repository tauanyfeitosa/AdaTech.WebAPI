
using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects;
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
            builder.Services.AddScoped<IRepository<Cliente>, ClienteRepository>();
            builder.Services.AddScoped<IRepository<ItemVenda>, ItemVendaRepository>();
            builder.Services.AddScoped<IRepository<DevolucaoTroca>, DevolucaoTrocaRepository>();
            builder.Services.AddScoped<IRepository<Venda>, VendaRepository>();
            builder.Services.AddScoped<IRepository<Produto>, ProdutoRepository>();

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlite(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }

}