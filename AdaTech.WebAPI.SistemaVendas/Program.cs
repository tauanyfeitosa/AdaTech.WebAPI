
using AdaTech.WebAPI.DadosLibrary.Data;
using AdaTech.WebAPI.DadosLibrary.DTO.Objects;
using AdaTech.WebAPI.DadosLibrary.Repository;
using AdaTech.WebAPI.DadosLibrary.Repository.RepositoryObjects;
using AdaTech.WebAPI.SistemaVendas.Utilities.Attributes.Swagger;
using AdaTech.WebAPI.SistemaVendas.Utilities.Middleware;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.DeleteInterface;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.GenericsService;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.ObjectService;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.ObjectService.EnderecoServiceCRUD;
using AdaTech.WebAPI.SistemaVendas.Utilities.Services.ObjectService.VendaServiceCRUD;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace AdaTech.WebAPI.SistemaVendas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sistema de Vendas", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }

                    var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                    if (controllerActionDescriptor != null)
                    {
                        var displayNameAttribute = controllerActionDescriptor.ControllerTypeInfo
                            .GetCustomAttributes(typeof(SwaggerDisplayNameAttribute), true)
                            .FirstOrDefault() as SwaggerDisplayNameAttribute;

                        if (displayNameAttribute != null)
                        {
                            return new[] { displayNameAttribute.DisplayName };
                        }
                    }

                    return new[] { api.ActionDescriptor.RouteValues["controller"] };
                });
                });

            builder.Services.AddScoped<EnderecoService>();
            builder.Services.AddScoped<ClienteService>();
            builder.Services.AddScoped<VendaCreateService>();
            builder.Services.AddScoped<VendaUpdateService>();
            builder.Services.AddScoped(typeof(GenericsGetService<>));
            builder.Services.AddScoped(typeof(GenericsDeleteService<>));
            builder.Services.AddScoped(typeof(HardDeleteStrategy<>));
            builder.Services.AddScoped(typeof(SoftDeleteStrategy<>));
            builder.Services.AddScoped<VendaSoftDeleteStrategy>();
            builder.Services.AddScoped<VendaHardDeleteStrategy>();
            builder.Services.AddScoped<EnderecoSoftDeleteStrategy>();
            builder.Services.AddScoped<EnderecoHardDeleteStrategy>();
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

            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}