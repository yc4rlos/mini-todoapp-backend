using AutoMapper;
using mesha_test_backend.Data;
using mesha_test_backend.Data.Dtos;
using mesha_test_backend.Models;

namespace mesha_test_backend.Services;

public class RefreshTokensService
{
    private readonly TasksDatabaseContext _dbContext;
    
    public RefreshTokensService(TasksDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public RefreshToken Create(string userId)
    {

        var refreshToken = new RefreshToken()
        {
            UserId =Guid.Parse(userId)
        };

        _dbContext.RefreshTokens.Add(refreshToken);
        _dbContext.SaveChanges();

        return refreshToken;

    }

    public RefreshToken? FindOneById(string id)
    {
        var refreshToken = _dbContext.RefreshTokens.FirstOrDefault(t => t.Id.ToString() == id);

        if (refreshToken == null) return null;

        return refreshToken;

    }

    public void Delete(string id)
    {
        var refreshToken = FindOneById(id);

        if (refreshToken == null) return;

        _dbContext.RefreshTokens.Remove(refreshToken);
    }
}