﻿

namespace Aiursoft.NugetNinja.Core;

public class ProjectsEnumerator
{
    public IEnumerable<Project> EnumerateAllBuiltProjects(Project input, bool includeSelf = true)
    {
        if (includeSelf)
        {
            yield return input;
        }
        foreach (var subProject in input.ProjectReferences)
        {
            foreach (var result in EnumerateAllBuiltProjects(subProject))
            {
                yield return result;
            }
        }
    }
}
