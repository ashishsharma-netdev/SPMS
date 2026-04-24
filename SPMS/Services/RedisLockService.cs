using StackExchange.Redis;
using System.Collections.Concurrent;

namespace SPMS.Services
{
 public class RedisLockService
 {
 private readonly IConnectionMultiplexer _conn;
 private readonly IDatabase _db;
 private readonly ConcurrentDictionary<string, string> _tokens = new();
 public RedisLockService(IConnectionMultiplexer conn) { _conn = conn; _db = _conn.GetDatabase(); }

 // Acquire lock with unique token stored locally
 public async Task<bool> AcquireLockAsync(string key, TimeSpan ttl)
 {
 var token = Guid.NewGuid().ToString();
 var ok = await _db.StringSetAsync(key, token, ttl, when: When.NotExists);
 if (ok) _tokens[key] = token;
 return ok;
 }

 // Release only if token matches
 public async Task<bool> ReleaseLockAsync(string key)
 {
 if (!_tokens.TryRemove(key, out var token)) return false;
 // Lua script to release lock only if value matches
 var script = @"if redis.call('get', KEYS[1]) == ARGV[1] then return redis.call('del', KEYS[1]) else return 0 end";
 var res = (int)await _db.ScriptEvaluateAsync(script, new RedisKey[] { key }, new RedisValue[] { token });
 return res == 1;
 }
 }
}