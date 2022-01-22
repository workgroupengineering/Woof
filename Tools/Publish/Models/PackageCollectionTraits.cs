﻿namespace Publish.Models;

public static class PackageCollectionTraits {

    public static IEnumerable<PackageNode> All(this ObservableList<PackageNode> items) => items.TraverseDFS(i => i.Dependencies);

    public static async ValueTask GetAsync(this ObservableList<PackageNode> items) {
        await foreach (var package in new LocalRepository().GetPackagesAsync()) {
            var lastVersion = (await package.GetVersionsAsync())?.Max(i => i.Version)?.ToString();
            var name = package.Identity.Id;
            var version = lastVersion;
            ObservableList<PackageNode>? dependencies = null;
            if (package.DependencySets.FirstOrDefault(i => i.TargetFramework.DotNetFrameworkName == Settings.Default.DotNetFrameworkName) is PackageDependencyGroup group) {
                foreach (var dependency in group.Packages) {
                    if (dependency.Id.StartsWith(Settings.Default.Prefix, StringComparison.Ordinal)) {
                        if (dependencies is null) dependencies = new();
                        dependencies.Add(new(dependency.Id, dependency.VersionRange.OriginalString, null));
                    }
                }
            }
            items.Add(new(name, version!, dependencies));
        }
        var reference = items.ToArray();
        foreach (var item in reference.TraverseDFS(i => i.Dependencies)) {
            var direct = reference.First(i => i.Name == item.Name);
            if (item.Dependencies is null && direct.Dependencies is not null) {
                item.Dependencies = new ObservableList<PackageNode>();
                foreach (var d in direct.Dependencies) item.Dependencies.Add(new(d));
            }
        }
    }

    public static IEnumerable<PackageItem> GetChecked(this IEnumerable<PackageNode> items) {
        List<PackageNode> result = new();
        foreach (var item in items.TraverseDFS(n => n.Dependencies).Where(i => i.IsChecked))
            if (!result.Any(i => i.Name == item.Name)) result.Add(item);
        return result;
    }

}