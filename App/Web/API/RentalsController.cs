using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repo.Context;
using Repo.Data.Queries;
using Helpers.BSTable;
using Swashbuckle.AspNetCore.Annotations;
using shortid;
using shortid.Configuration;

namespace WebApp.API;

[ApiController]
[Route("api/[controller]")]
public class RentalsController : ControllerBase
{
    private readonly TestLawenconSqliteContext _ctx;

    public RentalsController()
    {
        _ctx = new TestLawenconSqliteContext(new DbContextOptions<TestLawenconSqliteContext>());
    }

    /// <summary>
    ///     Contoh API dengan query menggunakan LinQ
    /// </summary>
    /// <param name="id">ID Transactions</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<Repo.Domain.Models.RentReadModel>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        try
        {
            var query = from trans in _ctx.Rents
                        join locker in _ctx.Lockers
                            on trans.LockerId equals locker.Id
                        where
                            trans.CustomerId == id
                            &&
                            trans.Status == 0
                        orderby 
                            trans.RentDate
                        select
                            new
                            {
                                trans.Id,
                                trans.CustomerId,
                                trans.LockerId,
                                locker.Number,
                                trans.RentDate
                            };

            var result = await query.ToListAsync();
            return result is not null && result.Any() ? Ok(result) : throw new Exception("This customer does not have active rents data.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    /// <summary>
    ///     Contoh API dengan menggunakan Lambda Expression &amp; SQL View
    /// </summary>
    /// <param name="param">Data parameter pagination</param>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<Repo.Domain.Models.VwRentTransReportReadModel>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Request param)
    {
        try
        {
            var query = _ctx.VwRentTransReports.AsQueryable();
            var result = new Response<Repo.Data.Entities.VwRentTransReport>
            {
                TotalNotFiltered = await query.CountAsync()
            };

            if (!string.IsNullOrWhiteSpace(param.Search) && !string.IsNullOrEmpty(param.Search))
            {
                query = query
                    .Where(
                        x =>
                            x.Name!.Contains(param.SearchUpper())
                            ||
                            x.TotalRentTrans.ToString() == param.Search
                            ||
                            x.TotalActiveRent.ToString() == param.Search
                            ||
                            x.TotalReturned.ToString() == param.Search
                            ||
                            x.TotalPaidFine.ToString() == param.Search
                    );
            }

            if (!string.IsNullOrWhiteSpace(param.Sort))
            {
                switch (param.Sort)
                {
                    case "customerId":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.CustomerId) : query.OrderBy(x => x.CustomerId); break;

                    case "name":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name); break;

                    case "totalRentTrans":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.TotalRentTrans) : query.OrderBy(x => x.TotalRentTrans); break;

                    case "totalActiveRent":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.TotalActiveRent) : query.OrderBy(x => x.TotalActiveRent); break;

                    case "totalReturned":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.TotalReturned) : query.OrderBy(x => x.TotalReturned); break;

                    case "totalPaidFine":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.TotalPaidFine) : query.OrderBy(x => x.TotalPaidFine); break;

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


    /// <summary>
    ///     Contoh API dengan menggunakan SQL Query (hard code)
    /// </summary>
    /// <returns></returns>
    [HttpGet("AvailLockersToRent")]
    public async Task<IActionResult> AvailLockersToRent()
    {
        try
        {
            var query = _ctx.Lockers.FromSqlRaw(
                @"
                    SELECT 
                        * 
                    FROM 
                        Locker AS l 
                    WHERE 
                        l.Id NOT in (SELECT r.LockerId FROM Rent r WHERE r.Status = 0)
                "
            );
            var result = await query.ToListAsync();

            if (result is null || result.Count < 1) throw new Exception("There are no locker available to rent");
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Repo.Domain.Models.RentUpdateModel rentInfo)
    {
        try
        {
            if (rentInfo is null) throw new ArgumentNullException(nameof(rentInfo));

            var cData = await _ctx.Rents.GetByKeyAsync(rentInfo.Id);
            if (cData is null) throw new Exception("Can not process unregistered rent transactions.");
            if (cData.Status != 0) throw new Exception("Can not process closed transactions.");
            if (string.IsNullOrWhiteSpace(rentInfo.Password) && rentInfo.TotalFine < 25000) throw new Exception("Can not process without password and fine.");
            if (!string.IsNullOrWhiteSpace(rentInfo.Password) && rentInfo.Password != cData.Password) throw new Exception("Password does not match.");

            cData.ReturnDate = DateTime.Now.ToString("yyyy-MM-dd");
            cData.TotalFine = rentInfo.TotalFine;

            if (string.IsNullOrWhiteSpace(rentInfo.Password) && rentInfo.TotalFine > 25000) cData.Status = 4;
            else if (string.IsNullOrWhiteSpace(rentInfo.Password) && rentInfo.TotalFine == 25000) cData.Status = 3;
            else if (!string.IsNullOrWhiteSpace(rentInfo.Password) && rentInfo.TotalFine > 0) cData.Status = 2;
            else cData.Status = 1;

            await _ctx.SaveChangesAsync();

            return Ok(cData);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Repo.Domain.Models.RentCreateModel rentInfo)
    {
        try
        {
            if (rentInfo is null) throw new ArgumentNullException(nameof(rentInfo));

            if (!await _ctx.Customers.AnyAsync(x => x.Id.Equals(rentInfo.CustomerId))) throw new Exception("Can not create rent transaction for unregistered customer.");
            if (!await _ctx.Lockers.AnyAsync(x => x.Id.Equals(rentInfo.LockerId))) throw new Exception("Can not create rent transaction for unregistered locker.");
            if (await _ctx.Rents.AnyAsync(x => x.LockerId.Equals(rentInfo.LockerId) && x.Status == 0)) throw new Exception("This locker is already rented.");
            if (await _ctx.Rents.CountAsync(x => x.CustomerId.Equals(rentInfo.CustomerId) && x.Status == 0) >= 3) throw new Exception("This customer is not eligible to rent another locker.");

            var nData = new Repo.Data.Entities.Rent
            {
                CustomerId = rentInfo.CustomerId,
                LockerId = rentInfo.LockerId,
                RentDate = DateTime.Now.ToString("yyyy-MM-dd"),
                Password = ShortId.Generate(new GenerationOptions(useNumbers: true, useSpecialCharacters: false, length: 8))
            };

            _ctx.Rents.Add(nData);
            await _ctx.SaveChangesAsync();

            return Ok(nData);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}