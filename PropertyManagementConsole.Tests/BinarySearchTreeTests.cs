using PropertyManagementConsole.DataStructures;
using PropertyManagementConsole.Models;

namespace PropertyManagementConsole.Tests;

[TestClass]
public class BinarySearchTreeTests
{
    [TestMethod]
    public void InsertAndSearch_ReturnsInsertedTenant()
    {
        var tree = new BinarySearchTree<Tenant>();
        var tenant = new Tenant { TenantId = 2, FullName = "Aanchal", FlatId = 4 };

        tree.Insert(tenant.TenantId, tenant);
        var result = tree.Search(2);

        Assert.IsNotNull(result);
        Assert.AreEqual("Aanchal", result.FullName);
        Assert.AreEqual(4, result.FlatId);
    }

    [TestMethod]
    public void Search_ForMissingTenant_ReturnsNull()
    {
        var tree = new BinarySearchTree<Tenant>();
        tree.Insert(1, new Tenant { TenantId = 1, FullName = "Krish", FlatId = 1 });

        var result = tree.Search(99);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Insert_WithDuplicateKey_OverwritesExistingValue()
    {
        var tree = new BinarySearchTree<Tenant>();
        tree.Insert(5, new Tenant { TenantId = 5, FullName = "Old Name", FlatId = 5 });
        tree.Insert(5, new Tenant { TenantId = 5, FullName = "Updated Name", FlatId = 6 });

        var result = tree.Search(5);

        Assert.IsNotNull(result);
        Assert.AreEqual("Updated Name", result.FullName);
        Assert.AreEqual(6, result.FlatId);
    }

    [TestMethod]
    public void Insert_MultipleValues_SearchesLeftAndRightBranchesCorrectly()
    {
        var tree = new BinarySearchTree<string>();
        tree.Insert(10, "root");
        tree.Insert(4, "left");
        tree.Insert(15, "right");
        tree.Insert(2, "left-left");
        tree.Insert(20, "right-right");

        Assert.AreEqual("left-left", tree.Search(2));
        Assert.AreEqual("left", tree.Search(4));
        Assert.AreEqual("root", tree.Search(10));
        Assert.AreEqual("right", tree.Search(15));
        Assert.AreEqual("right-right", tree.Search(20));
    }
}
