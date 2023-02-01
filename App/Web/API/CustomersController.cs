using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repo.Context;
using Repo.Data.Queries;
using Helpers.BSTable;

namespace WebApp.API;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly TestLawenconSqliteContext _ctx;

    public CustomersController()
    {
        _ctx = new TestLawenconSqliteContext(new DbContextOptions<TestLawenconSqliteContext>());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        try
        {
            var result = await _ctx.Customers.GetByKeyAsync(id);
            return result is not null ? Ok(result) : throw new Exception("Customer is not registered.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Request param)
    {
        try
        {
            var query = _ctx.Customers.AsQueryable();
            var result = new Response<Repo.Data.Entities.Customer>
            {
                TotalNotFiltered = await query.CountAsync()
            };

            if (!string.IsNullOrWhiteSpace(param.Search) && !string.IsNullOrEmpty(param.Search))
            {
                query = query
                    .Where(
                        x =>
                            x.Name.Contains(param.SearchUpper())
                            ||
                            x.Hp.Contains(param.SearchUpper())
                            ||
                            x.Nik.Contains(param.SearchUpper())
                            ||
                            x.Email.Contains(param.SearchUpper())
                    );
            }

            if (!string.IsNullOrWhiteSpace(param.Sort))
            {
                switch (param.Sort)
                {
                    case "id":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id); break;

                    case "name":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name); break;

                    case "hp":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.Hp) : query.OrderBy(x => x.Hp); break;

                    case "nik":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.Nik) : query.OrderBy(x => x.Nik); break;

                    case "email":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email); break;

                    default: break;
                }
            }

            result.Total = await query.CountAsync();
            result.Rows = await query.Skip(param.Offset).Take(param.Limit).ToListAsync();

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var result = await _ctx.Customers.GetByKeyAsync(id);
            if (result is null) throw new Exception("Customer is not registered");

            _ctx.Customers.Remove(result);
            await _ctx.SaveChangesAsync();

            return Ok("Deleted successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Repo.Domain.Models.CustomerCreateModel custInfo)
    {
        try
        {
            if (custInfo is null) throw new ArgumentNullException(nameof(custInfo));

            var checkExistResult = await _ctx.Customers.CheckExistingsForNew(custInfo);
            if (!string.IsNullOrEmpty(checkExistResult)) throw new Exception(checkExistResult);

            var nCust = new Repo.Data.Entities.Customer
            {
                Name = custInfo.Name.ToUpper(),
                Email = custInfo.Email.ToUpper(),
                Nik = custInfo.Nik.ToUpper(),
                Hp = custInfo.Hp.ToUpper()
            };

            await _ctx.Customers.AddAsync(nCust);
            await _ctx.SaveChangesAsync();

            return Ok(nCust);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Repo.Domain.Models.CustomerUpdateModel custInfo)
    {
        try
        {
            if (custInfo is null) throw new ArgumentNullException(nameof(custInfo));

            var cData = await _ctx.Customers.GetByKeyAsync(custInfo.Id);
            if (cData is null) throw new Exception("Can not update unregistered customer.");

            var checkExistResult = await _ctx.Customers.CheckExistingsForUpdate(custInfo);
            if (!string.IsNullOrEmpty(checkExistResult)) throw new Exception(checkExistResult);

            cData.Name = custInfo.Name.ToUpper();
            cData.Hp = custInfo.Hp.ToUpper();
            cData.Nik = custInfo.Nik.ToUpper();
            cData.Email = custInfo.Email.ToUpper();
            await _ctx.SaveChangesAsync();

            return Ok(cData);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}