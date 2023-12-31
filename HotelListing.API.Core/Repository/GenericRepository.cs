﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Core.Models;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Core.Exceptions;

namespace HotelListing.API.Core.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbContext _dbContext;
        private readonly IMapper _mapper;

        public GenericRepository(HotelListingDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            var entity = _mapper.Map<T>(source);

            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            
            return _mapper.Map<TResult>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            return await _dbContext.Set<T>().ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            var totalSize = await _dbContext.Set<T>().CountAsync();
            var items = await _dbContext.Set<T>()
                .Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<TResult>
            {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                RecordNumber = queryParameters.PageSize,
                TotalCount = totalSize
            };
        }

        public async Task<T> GetAsync(int? id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            if (entity is null)
                throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No key provided");

            return entity;
        }

        public async Task<TResult> GetAsync<TResult>(int? id)
        {
            var entity = await GetAsync(id);

            return _mapper.Map<TResult>(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync<TSource>(int id, TSource source)
        {
            var entity = await GetAsync(id);
            entity = _mapper.Map(source, entity);
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
