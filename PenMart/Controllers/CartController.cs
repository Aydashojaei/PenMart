using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PenMart.Data;
using PenMart.Data.Repositories;
using PenMart.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PenMart.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository _productRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PenMartContext _context;

        public CartController(IProductRepository productRepository, UserManager<ApplicationUser> userManager, PenMartContext context)
        {
            _context = context;
            _productRepository = productRepository;
            _userManager = userManager;
        }

        [Authorize]
        public  IActionResult AddToCart(int Id)
        {
            var Product = _productRepository.GetProductById(Id);
            if (Product != null)
            {
                var userId = _userManager.GetUserId(User);
                var order = _context.Orders.FirstOrDefault(o => o.UserId == userId && o.IsFinaly == false);
                if (order != null)
                {
                    var orderDetail = _context.OrderDetails.FirstOrDefault(o => o.OrderId == order.OrderId && o.ProductId == Product.Id);
                    if (orderDetail != null)
                    {
                        orderDetail.Count += 1;

                    }
                    else
                    {
                        _context.OrderDetails.Add(new OrderDetail()
                        {
                            OrderId = order.OrderId,
                            ProductId = Product.Id,
                            Price = Product.Item.Price,
                            Count = 1

                        });
                    }

                }
                else
                {
                    order = new Order()
                    {
                        CreatDate = System.DateTime.Now,
                        UserId = userId,
                        IsFinaly = false
                    };
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    _context.OrderDetails.Add(new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        ProductId = Product.Id,
                        Price = Product.Item.Price,
                        Count = 1

                    });
                }
                _context.SaveChanges();

            }
            return RedirectToAction("ShowCart");
        }
        public IActionResult ShowCart()
        {
            var userId = _userManager.GetUserId(User);
            var order = _context.Orders.Include(d => d.orderDetails)
                .ThenInclude(d => d.product)
                .ThenInclude(d=> d.Images)
                .FirstOrDefault(o => o.UserId == userId && o.IsFinaly == false);

            return View(order);
        }

        public IActionResult RemoveCart(int detailId)
        {
            var orderDetail = _context.OrderDetails.FirstOrDefault(o=> o.DetailId==detailId);
            
                _context.OrderDetails.Remove(orderDetail);
                _context.SaveChanges();
          
            return RedirectToAction("ShowCart");
        }
    }
}
