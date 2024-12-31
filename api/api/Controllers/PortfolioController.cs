using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;
[Route("api/portfolio")]
public class PortfolioController : ControllerBase
{
    private readonly UserManager <AppUser> _userManager;
    private readonly IStockRepository _stockRepo;
    private readonly IPortfolioRepository _portfolioRepo;
    
    public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
    {
        _userManager = userManager;
        _stockRepo = stockRepo;
        _portfolioRepo = portfolioRepo;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPorfolio()
    {
        var username = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPorfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        return Ok(userPorfolio);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(username);
        var stock = await _stockRepo.GetBySymbolAsync(symbol);

        if (stock == null) return BadRequest("Stock not found");
        
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot add duplicate stock to portfolio");

        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id,
        };
        await _portfolioRepo.CreateAsync(portfolioModel);
        
        if (portfolioModel == null) return StatusCode(500, "Failed to add stock to portfolio");

        return Created();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUserName();
        var appUser = await _userManager.FindByNameAsync(username);
        
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

        var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

        if (filteredStock.Count() == 1)
        {
            await _portfolioRepo.DeletePortfolio(appUser, symbol);
        }
        else
        {
            return BadRequest("Stock not found in portfolio");
        }

        return Ok();
    }
}
