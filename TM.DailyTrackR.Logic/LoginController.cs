﻿namespace TM.DailyTrackR.Logic;

public sealed class LoginController
{
    private readonly string USERNAME = "admin";
    private readonly string PASSWORD = "1234";

    public async Task<bool> Login(String username, String password)
    {
        if (username != USERNAME) return false;
        if (password != PASSWORD) return false;

        return true;
    }
}