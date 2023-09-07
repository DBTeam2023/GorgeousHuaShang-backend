using EntityFramework.Context;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Product.exception;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Product.dto;
using Microsoft.AspNetCore.Mvc;
using Product.domain.model;
using Product.domain.service.impl;
using Product.domain.service;
using System.Security.Cryptography.X509Certificates;

namespace Product.domain.model.repository.impl
{
    public class CartRepositoryImpl : CartRepository
    {
        private readonly ModelContext _context;
        private ProductRepository _productRepository;
        public CartRepositoryImpl(ModelContext context, ProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

        //添加购物车表
        public async Task add(string userId)
        {
            var db_user = _context.Users.Where(c => c.UserId == userId).FirstOrDefault();
            if (db_user == null)
            {
                throw new NotFoundException("The user doesn't existed!");
            }
            var db_cart = _context.Carts.Where(c => c.UserId == userId).FirstOrDefault();
            if (db_cart != null)
            {
                throw new DuplicateException("The cart has already existed!");
            }
            IDbContextTransaction? tran = null;
            var new_cart = new Cart
            {
                TotalQuantity = 0,
                TotalAmount = 0,
                UserId =userId
            };
            try
            {
                tran = _context.Database.BeginTransaction();
                await _context.Carts.AddAsync(new_cart);
                await _context.SaveChangesAsync();
                await tran.CommitAsync();
            }
            catch
            {
                if (tran != null)
                    tran.Rollback();
                throw new DBFailureException("create cart faliure");
            }
            finally
            {
                if (tran != null)
                {
                    tran.Dispose();
                }
            }
        }

        public async Task addItem(string userID,PickCountDto pick)
        {
            //To do change cart
            var items = _context.CartPicks.Where(c => c.PickId==pick.PickId&&c.UserId==userID).FirstOrDefault();
            var db_pick = _context.Picks.Where(c => c.PickId == pick.PickId).FirstOrDefault();
            var db_cart = _context.Carts.Where(c => c.UserId == userID).FirstOrDefault();
            if(db_pick==null)
            {
                throw new NotFoundException("the pick doesn't exist");
            }
            if (items != null&&db_cart!=null)
            {
                items.PickCount++;
                db_cart.TotalAmount += (db_pick.Price ?? 0) * pick.Number;
                await _context.SaveChangesAsync();
            }
            else
            {
                IDbContextTransaction? tran = null;
                var new_cart_pick = new CartPick
                {
                    UserId = userID,
                    PickCount = pick.Number,
                    PickId = pick.PickId
                };
                
                try
                {
                    tran = _context.Database.BeginTransaction();
                    await _context.CartPicks.AddAsync(new_cart_pick);
                    db_cart.TotalQuantity++;
                    db_cart.TotalAmount += (db_pick.Price ?? 0) * pick.Number;
                    await _context.SaveChangesAsync();
                    await tran.CommitAsync();
                }
                catch
                {
                    if (tran != null)
                        tran.Rollback();
                    throw new DBFailureException("create cart_pick faliure");
                }
                finally
                {
                    if (tran != null)
                        tran.Dispose();
                }
            }
           
        }

        public async Task changePick(string userID, ChangePickDto changePickDto)
        {
            var db_item = _context.CartPicks.Where(c => c.PickId == changePickDto.oldPickId && c.UserId == userID).FirstOrDefault();
            if (db_item == null)
            {
                throw new NotFoundException("the item doesn't exist in cart");
            }

            var db_pick = _context.Picks.Where(c => c.PickId == changePickDto.newPickId).FirstOrDefault();

            //该pick已失效
            if(db_pick == null)
            {
                throw new NotFoundException("the pick has been removed");
            }
            // var changePick = cartAggregate.Picks.FirstOrDefault(p => p.PickId == db_item.PickId);
            var db_newitem =_context.CartPicks.Where(c => c.PickId == changePickDto.newPickId && c.UserId == userID).FirstOrDefault();
            if (db_newitem!=null)
            {
                //该pick已经存在
                db_newitem.PickCount += changePickDto.count;
                // await _context.CartPicks.Remove(db_item);
                _context.Remove(db_item);
                await _context.SaveChangesAsync();
            }

            else
            {
                IDbContextTransaction? tran = null;
                var new_cart_pick = new CartPick
                {
                    UserId = userID,
                    PickCount = changePickDto.count,
                    PickId = changePickDto.newPickId
                };
                try
                {
                    tran = _context.Database.BeginTransaction();
                    await _context.CartPicks.AddAsync(new_cart_pick);
                    _context.Remove(db_item);
                    await _context.SaveChangesAsync();
                    await tran.CommitAsync();
                }
                catch
                {
                    if (tran != null)
                        tran.Rollback();
                    throw new DBFailureException("create cart_pick faliure");
                }
                finally
                {
                    if (tran != null)
                        tran.Dispose();
                }
            }
        }

        
        public async Task updateCart(CartAggregate cartAggregate)
        {
            var db_cart = _context.Carts.Where(c => c.UserId == cartAggregate.UserId).FirstOrDefault();
            if (db_cart == null)
                throw new NotFoundException("The cart doesn't exist!");
            //change
            db_cart.TotalQuantity = cartAggregate.Total_quantity;
            db_cart.TotalAmount = cartAggregate.Total_amount;
            
            
            IDbContextTransaction? tran = null;
            try
            {
                tran = _context.Database.BeginTransaction();
                await _context.SaveChangesAsync();
                await tran.CommitAsync();
            }
            catch
            {
                if (tran != null)
                    tran.Rollback();
                throw new DBFailureException("update cart faliure");
            }
            finally
            {
                tran.Dispose();
            }
        }

        public async Task deletePick(string userID,string pickID)
        {
            var db_cart_pick = _context.CartPicks.Where(c => c.PickId == pickID&&c.UserId==userID).FirstOrDefault();
            if (db_cart_pick == null)
                throw new NotFoundException("The commodity doesn't exist");
            IDbContextTransaction? tran = null;
           
            var db_cart = _context.Carts.Where(c => c.UserId == userID).FirstOrDefault();
            //var del_pick= cartAggregate.Picks.FirstOrDefault(p => p.PickId == pickID);
            var del_pick = _context.Picks.Where(c => c.PickId == pickID).FirstOrDefault();

            //cart change
            
            //await _context.SaveChangesAsync();

            try
            {
                tran = _context.Database.BeginTransaction();
                 _context.CartPicks.Remove(db_cart_pick);
                db_cart.TotalQuantity--;
                db_cart.TotalAmount -= (del_pick.Price ?? 0) * db_cart_pick.PickCount;
                await _context.SaveChangesAsync();
                await tran.CommitAsync();
            }
            catch
            {
                if (tran != null)
                    tran.Rollback();
                throw new DBFailureException("update cart faliure");
            }
            finally
            {
                tran.Dispose();
            }
        }

        public async Task<string> getUserId(string token)
        {
            string userId;
            string url = "http://47.115.231.142:1025/UserIdentification/getUserInfo";

            HttpClient client = new HttpClient();
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", token);
                HttpResponseMessage response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                JObject code = JObject.Parse(responseBody);
                Console.WriteLine(code);

                // 获取 userId 字段的值
                userId = (string)code["data"]["userId"];

                Console.WriteLine(userId);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception(ex.Message);
            }
            if (userId == null)
                throw new NotFoundException("There is no match to this token.");

            return userId;
        }
    }
}
