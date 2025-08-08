using System;
using System.Collections.Generic;
using System.Linq;

public static class Pagination
{
    public static int PageIndex = 0;
    public static int PageSize = 10;

    public static int TotalPages(int totalItems)
    {
        return (int)Math.Ceiling((float)totalItems / PageSize);
    }

    public static List<T> GetPage<T>(List<T> allItems)
    {
        return allItems.Skip(PageIndex * PageSize).Take(PageSize).ToList();
    }

    public static void NextPage(int totalItems)
    {
        if (PageIndex < TotalPages(totalItems) - 1) PageIndex++;
    }

    public static void PrevPage()
    {
        if (PageIndex > 0) PageIndex--;
    }

    public static void Reset() => PageIndex = 0;
}