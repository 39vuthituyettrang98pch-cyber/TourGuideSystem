using AdminWeb.Data;
using AdminWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AdminWeb.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class LogApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public LogApiController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/log
    [HttpPost]
    public async Task<IActionResult> PostLog([FromBody] VisitorPlaybackLog log)
    {
        if (log == null)
        {
            return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
        }

        if (log.CreatedAt == default)
        {
            log.CreatedAt = DateTime.Now;
        }

        _context.VisitorPlaybackLogs.Add(log);
        await _context.SaveChangesAsync();
        return Ok(new { success = true, message = "Đã lưu lịch sử phát audio thành công." });
    }
}