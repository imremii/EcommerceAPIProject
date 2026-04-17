using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public static class BLLServicesExtention
    {
        public static void AddBLLServices(this IServiceCollection services)
        {
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IImageManager, ImageManager>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<ICartManager, CartManager>();

            services.AddValidatorsFromAssembly(typeof(BLLServicesExtention).Assembly);
        }
    }
}
