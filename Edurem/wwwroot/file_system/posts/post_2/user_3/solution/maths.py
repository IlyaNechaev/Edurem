def factorial(n):
    f = 1
    while n > 1:
        f *= n
        n -= 1
    return f