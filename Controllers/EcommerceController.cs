using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Services;
using EcommerceApi.DTOs;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EcommerceController : ControllerBase
{
    private readonly IEcommerceService _service;
    public EcommerceController(IEcommerceService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEcommerceDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var item = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEcommerceDto dto)
    {
        var item = await _service.UpdateAsync(id, dto);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
