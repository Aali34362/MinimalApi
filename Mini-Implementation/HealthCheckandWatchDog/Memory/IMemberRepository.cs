﻿namespace HealthCheckandWatchDog.Memory;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);

    void Add(Member member);

    void Update(Member member);
}
