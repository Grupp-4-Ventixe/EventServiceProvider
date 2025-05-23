﻿using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PackagesController(IPackageService packageService) : ControllerBase
{
    private readonly IPackageService _packageService = packageService;

    [HttpPost]
    public async Task<IActionResult> AddPackage([FromBody] CreatePackageFormData model)
    {
        var result = await _packageService.CreatePackageAsync(model);
        return StatusCode(result.StatusCode, result.Succeeded ? "Package added." : result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePackage(Guid id, [FromBody] UpdatePackageFormData model)
    {
        var result = await _packageService.UpdatePackageAsync(id, model);
        return StatusCode(result.StatusCode, result.Succeeded ? "Updated" : result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackage(Guid id)
    {
        var result = await _packageService.DeletePackageAsync(id);
        return StatusCode(result.StatusCode, result.Succeeded ? "Deleted" : result.Error);
    }

}