using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repo.Context;
using Repo.Data.Queries;
using Helpers.BSTable;

namespace WebApp.API;

[ApiController]
[Route("api/[controller]")]
public class LockersController : ControllerBase
{
    private readonly TestLawenconSqliteContext _ctx;

    public LockersController()
    {
        _ctx = new TestLawenconSqliteContext(new DbContextOptions<TestLawenconSqliteContext>());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        try
        {
            var result = await _ctx.Lockers.GetByKeyAsync(id);
            return result is not null ? Ok(result) : throw new Exception("Locker is not registered.");
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
            var query = _ctx.Lockers.AsQueryable();
            var result = new Response<Repo.Data.Entities.Locker>
            {
                TotalNotFiltered = await query.CountAsync()
            };

            if (!string.IsNullOrWhiteSpace(param.Search) && !string.IsNullOrEmpty(param.Search))
            {
                query = query.Where(x => x.Number.Contains(param.SearchUpper()));
            }

            if (!string.IsNullOrWhiteSpace(param.Sort))
            {
                switch (param.Sort)
                {
                    case "id":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id); break;

                    case "number":
                        query = param.Order != "asc" ? query.OrderByDescending(x => x.Number) : query.OrderBy(x => x.Number); break;

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
            var result = await _ctx.Lockers.GetByKeyAsync(id);
            if (result is null) throw new Exception("Locker is not registered.");

            _ctx.Lockers.Remove(result);
            await _ctx.SaveChangesAsync();

            return Ok("Deleted successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Repo.Domain.Models.LockerCreateModel lockInfo)
    {
        try
        {
            if (lockInfo is null) throw new ArgumentNullException(nameof(lockInfo));
            var checkExistResult = await _ctx.Lockers.CheckExistingForNew(lockInfo);
            if (!string.IsNullOrEmpty(checkExistResult)) throw new Exception(checkExistResult);

            var nLock = new Repo.Data.Entities.Locker
            {
                Number = lockInfo.Number.ToUpper()
            };

            await _ctx.Lockers.AddAsync(nLock);
            await _ctx.SaveChangesAsync();

            return Ok(nLock);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Repo.Domain.Models.LockerUpdateModel lockInfo)
    {
        try
        {
            if (lockInfo is null) throw new ArgumentNullException(nameof(lockInfo));

            var cData = await _ctx.Lockers.GetByKeyAsync(lockInfo.Id);
            if (cData is null) throw new Exception("Can not update unregistered locker.");

            var checkExistResult = await _ctx.Lockers.CheckExistingForUpdate(lockInfo);
            if (!string.IsNullOrEmpty(checkExistResult)) throw new Exception(checkExistResult);

            cData.Number = lockInfo.Number.ToUpper();
            await _ctx.SaveChangesAsync();

            return Ok(cData);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}