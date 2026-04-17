using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IProductRepository? _products;
        private ICartRepository? _carts;
        private IOrderRepository? _orders;
        private IGenericRepository<Category>? _categories;

        public UnitOfWork(AppDbContext context, IProductRepository productRepository, ICartRepository cartRepository, IOrderRepository orderRepository, IGenericRepository<Category> categoryRepository)
        {
            _context = context;
            _products = productRepository;
            _carts = cartRepository;
            _orders = orderRepository;
            _categories = categoryRepository;
        }

        public IProductRepository Products => _products ??= new ProductRepository(_context);
        public ICartRepository Carts => _carts ??= new CartRepository(_context);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_context);
        public IGenericRepository<Category> Categories => _categories ??= new GenericRepository<Category>(_context);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    }
}
