﻿using System.ComponentModel.DataAnnotations;
using Aiursoft.NugetNinja.Core;
using Aiursoft.NugetNinja.WebPortal.Models.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Aiursoft.NugetNinja.WebPortal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly GitHubService _gitHubService;

    public HomeController(
        ILogger<HomeController> logger,
        GitHubService gitHubService)
    {
        _logger = logger;
        _gitHubService = gitHubService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        _logger.LogInformation("A user accessed the index page.");

        var model = new IndexViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe([FromForm] IndexViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(viewName: nameof(Index), model: model);
        }

        var (org, repo) = model.GetGitHubValues();
        if (!await _gitHubService.RepoExists(org, repo))
        {
            ModelState.AddModelError(nameof(model.GitHubRepositoryUrl), $"The repository '{org}/{repo}' doesn't exist on GitHub!");
            return View(viewName: nameof(Index), model: model);
        }

        _logger.LogInformation($"A user subscribed the repo: {model.GitHubRepositoryUrl}");

        // Really subscribe.
        return RedirectToAction(nameof(SuccessfullySubscribed));
    }

    [HttpGet]
    public IActionResult SuccessfullySubscribed()
    {
        throw new NotImplementedException();
        // return view();
    }

    [HttpGet]
    public IActionResult ConsiderUnsubscribe([FromQuery] string? repo)
    {
        throw new NotImplementedException();
        // return view();
    }

    [HttpPost]
    public IActionResult Unsubscribe([FromForm][Required][Url] string githubUrl)
    {
        _logger.LogInformation($"A user unsubscribed the repo: {githubUrl}");

        // Really unsubscribe.
        return RedirectToAction(nameof(SuccessfullyUnSubscribed));
    }

    [HttpGet]
    public IActionResult SuccessfullyUnSubscribed()
    {
        throw new NotImplementedException();
        // return view();
    }
}
