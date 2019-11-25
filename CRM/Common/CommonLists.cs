using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public static class CommonLists
{
    public static List<SelectListItem> SexList()
    {
        List<SelectListItem> listItems = new List<SelectListItem>();
        listItems.Add(new SelectListItem
        {
            Text = "Masculino",
            Value = "M"
        });
        listItems.Add(new SelectListItem
        {
            Text = "Feminino",
            Value = "F",
        });
        return listItems;
    }

    public static List<SelectListItem> Classification()
    {
        List<SelectListItem> listItems = new List<SelectListItem>();
        listItems.Add(new SelectListItem
        {
            Text = "Vip",
            Value = "V"
        });
        listItems.Add(new SelectListItem
        {
            Text = "Regular",
            Value = "R",
        });

        listItems.Add(new SelectListItem
        {
            Text = "Esporádico",
            Value = "S",
        });
        return listItems;
    }

    
}






