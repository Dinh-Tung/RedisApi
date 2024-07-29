using ApiRedis.ApplicationServices.Interfaces;
using ApiRedis.DatabaseContext;
using ApiRedis.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ApiRedis.Controllers;

[ApiController]
[Route("api/[controller]/product")]
public class ProductController : ControllerBase
{
  private readonly DbContextClass _dbContext;
  private readonly ICacheService _cacheService;

  public ProductController(
    DbContextClass dbContext,
    ICacheService cacheService
  )
  {
    _dbContext = dbContext;
    _cacheService = cacheService;
  }

  [HttpGet("products")]
  public IEnumerable<Products> GetAll()
  {
    IEnumerable<Products> cacheData = _cacheService.GetData<IEnumerable<Products>>("product");
    if(cacheData != null) return cacheData;
    DateTimeOffset expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
    cacheData = _dbContext.Products.ToList();
    _cacheService.SetData<IEnumerable<Products>>("product", cacheData, expirationTime);
    return cacheData;
  }

  [HttpGet("product")]
  public Products? Get(
    int Id
  )
  {
    Products? filteredProduct = new ();
    IEnumerable<Products?> cacheData = _cacheService.GetData<IEnumerable<Products>>("product");
    if (cacheData != null)
    {
      filteredProduct = cacheData.FirstOrDefault(x => x.ProductId == Id);
      return filteredProduct;
    }
    filteredProduct = _dbContext.Products.FirstOrDefault(x => x.ProductId == Id);
    return filteredProduct;
  }
  
  [HttpPost("AddProduct")]
  public async Task < Products > Post(Products value) {
    EntityEntry<Products> obj = await _dbContext.Products.AddAsync(value);
    await _dbContext.SaveChangesAsync();
    _cacheService.RemoveData("product");
    return obj.Entity;
  }
  
  [HttpPut("EditProduct")]
  public int Put(Products product) {
    EntityEntry<Products> obj = _dbContext.Products.Update(product);
    _dbContext.SaveChanges();
    _cacheService.RemoveData("product");
    return obj.Entity.ProductId;
  }
  
  [HttpDelete("DeleteProduct")]
  public void Delete(int Id) {
    Products? filteredData = _dbContext.Products.FirstOrDefault(x => x.ProductId == Id);
    _dbContext.Remove(filteredData);
    _dbContext.SaveChanges();
    _cacheService.RemoveData("product");
  }

}