using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PenMart.Data;
using PenMart.Data.Repositories;
using PenMart.Models;
using System;
using System.Linq;

namespace PenMart.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PenMartContext _context;

        public CartController(
            IProductRepository productRepository,
            UserManager<ApplicationUser> userManager,
            PenMartContext context)
        {
            _context = context;
            _productRepository = productRepository;
            _userManager = userManager;
        }

        // ===================== ADD TO CART =====================

        [Authorize]
        public IActionResult AddToCart(int Id)
        {
            var product = _productRepository.GetProductById(Id);
            if (product == null)
                return RedirectToAction("ShowCart");

            var userId = _userManager.GetUserId(User);
            var order = _context.Orders
                .FirstOrDefault(o => o.UserId == userId && o.IsFinaly == false);

            if (order != null)
            {
                var orderDetail = _context.OrderDetails
                    .FirstOrDefault(o => o.OrderId == order.OrderId && o.ProductId == product.Id);

                if (orderDetail != null)
                {
                    orderDetail.Count += 1;
                }
                else
                {
                    _context.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = product.Id,
                        Price = product.Item.Price,
                        Count = 1
                    });
                }
            }
            else
            {
                order = new Order
                {
                    CreatDate = DateTime.Now,
                    UserId = userId,
                    IsFinaly = false
                };
                _context.Orders.Add(order);
                _context.SaveChanges();

                _context.OrderDetails.Add(new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = product.Id,
                    Price = product.Item.Price,
                    Count = 1
                });
            }

            _context.SaveChanges();
            return RedirectToAction("ShowCart");
        }

        // ===================== SHOW CART =====================

        [Authorize]
        public IActionResult ShowCart()
        {
            var userId = _userManager.GetUserId(User);

            var order = _context.Orders
                .Include(d => d.orderDetails)
                    .ThenInclude(d => d.product)
                        .ThenInclude(d => d.Images)
                .FirstOrDefault(o => o.UserId == userId && o.IsFinaly == false);

            return View(order);
        }

        // ===================== REMOVE FROM CART =====================

        [Authorize]
        public IActionResult RemoveCart(int detailId)
        {
            var orderDetail = _context.OrderDetails
                .FirstOrDefault(o => o.DetailId == detailId);

            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                _context.SaveChanges();
            }

            return RedirectToAction("ShowCart");
        }

        // ===================== CHECKOUT =====================

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = _userManager.GetUserId(User);

            var order = _context.Orders
                .Include(o => o.orderDetails)
                    .ThenInclude(d => d.product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefault(o => o.UserId == userId && o.IsFinaly == false);

            // If cart is empty, go back
            if (order == null || !order.orderDetails.Any())
            {
                TempData["Error"] = "سبد خرید شما خالی است.";
                return RedirectToAction("ShowCart");
            }

            var vm = new CheckoutViewModel
            {
                Order = order,
                FullName = User.Identity.Name
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(CheckoutViewModel vm)
        {
            var userId = _userManager.GetUserId(User);

            var order = _context.Orders
                .Include(o => o.orderDetails)
                    .ThenInclude(d => d.product)
                        .ThenInclude(p => p.Images)
                .FirstOrDefault(o => o.UserId == userId && o.IsFinaly == false);

            if (order == null || !order.orderDetails.Any())
                return RedirectToAction("ShowCart");

            // Re-attach order for display if validation fails
            vm.Order = order;

            if (!ModelState.IsValid)
                return View(vm);

            // Finalize the order
            order.IsFinaly = true;
            _context.SaveChanges();

            return RedirectToAction("OrderSuccess", new { orderId = order.OrderId });
        }

        // ===================== ORDER SUCCESS =====================

        [Authorize]
        public IActionResult OrderSuccess(int orderId)
        {
            var userId = _userManager.GetUserId(User);

            var order = _context.Orders
                .Include(o => o.orderDetails)
                    .ThenInclude(d => d.product)
                .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
                return RedirectToAction("Index", "Home");

            return View(order);
        }

        // ===================== MY ORDERS =====================

        [Authorize]
        public IActionResult MyOrders()
        {
            var userId = _userManager.GetUserId(User);

            var orders = _context.Orders
                .Include(o => o.orderDetails)
                    .ThenInclude(d => d.product)
                        .ThenInclude(p => p.Images)
                .Where(o => o.UserId == userId && o.IsFinaly == true)
                .OrderByDescending(o => o.CreatDate)
                .ToList();

            return View(orders);
        }
    }
}
