using Ecommerce.BLL;
using FluentValidation;

namespace Ecommerce.API
{
    public static class ValidationExtention
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
            return services;
        }
    }
}
