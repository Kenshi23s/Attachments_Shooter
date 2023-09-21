using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyManager
{
    public static int ActualCurrency = 1000;
    public static int maxDebt;

    public static bool InDebt => ActualCurrency < 0;
    public static int HowMuchDebt => ActualCurrency < 0 && maxDebt == 0 ? ActualCurrency * -1 : 0;

    public static event Action OnBalanceChange = delegate { };

    public static bool ConfirmPurchase(int priceToPay)
    {
        if (!CanBuy(priceToPay, out _)) return false;

        RemoveCurrency(priceToPay);
        return true;
    }

    public static void RemoveCurrency(int currencyToRemove)
    {
        currencyToRemove = Mathf.Abs(currencyToRemove);

        ActualCurrency = Mathf.Max(ActualCurrency - currencyToRemove, Mathf.Abs(maxDebt) * -1);
        OnBalanceChange();

    }

    public static bool CanBuy(int price, out int difference)
    {
       
        if (price > ActualCurrency)
        {
            difference = ActualCurrency - price;
            return false;
        }
        difference = 0;
        return true;
    }
}
