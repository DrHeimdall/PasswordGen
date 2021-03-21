# Random String Generator

This program is intended as a command line utility. It has a few quirky features and could maybe be useful to you if you have a unix or unix-like os.

Disclaimer: This was a really quick project that took like 20 minuets, I probably won't update it.


## Multi-threaded

Yes, you read that correctly. There really is no point to this because to generate intMax characters would only take approx 9.21 seconds on my computer (then like 10 years to print those characters to the console lol). Any strings less than 15000 characters will use single-threaded mode.

I only did this because I wanted to refresh my knowledge of how to do that stuff.


# Arguments and Usage

```
rnd [help] [-h] [-t] [-c] [-n] <<len...> or <max> or <min max...>>

Prints random sequence(s) of characters of length len
Or if in number mode prints a random number between
min and max inclusive. If a single number is given, min is 0

Numbers can be integer or decimal, or in the format of
  - x^y x and y can be decimal or integer. Returns a decimal
  - xEy x can be either but y is integer. Returns a decimal
  - If either min or max is decimal the result will be too

Arguments:
  -h Prints human characters (easily typed ones)
    -h uses:
    qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789
    default uses:
    qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789!"£$%^&*()-_=+[]{}#~`¬|\<>,.?/™

  -t Prints the amount of time taken to fulfil the request
  -c Prints in bash mode with a user-unfriendly output
  -l Uses newlines to separate data rather than , in bash mode
  -n Puts the generator in number mode
    -h is ignored in number mode, obviously
```

You can generate many different passwords by giving multiple lengths. They will all use different secure-random seeds.

## Example Usage

```
$ rnd 15 15 15
Password #1: m0™1U`*m%8lPfL4
Password #2: OsgQ&FBG1~AJf~q
Password #3: v$}a"Jq}/P_0#%j
```

```
$ rnd -t -h 15 15 15
Calculated in 0.0036509 seconds
Password #1: QlyqLPLT9WG6NKn
Password #2: qwGFo6HooYR7zdR
Password #3: R3UvvCPWCKyA3PB
```

```
$ rnd 15 -c -h 30 5 3
YbMZfaGwpgWaSQu,kvg0LiQVfLROhFPG9GRHjpWULbCkVv,FO3xU,DMS
```

```
$ rnd 15 15 -l -c -h
JulCBdiuLZrMKRu
Zu27riHVU8ub9kL
```

```
$ rnd -c -h -t 15
0.0036382
Zd7i8ASbrHRr85U
```

```
$ rnd -n 1
Number #1: 1
```

```
$ rnd -n 0 1.0 0 1. 0 1
Number #1: 0.177324498620501
Number #2: 0.6010764365089482
Number #3: 1
```

```
$ rnd -n 2^8 -c
157.02965159575905
```

```
# When there is a minus sign in front of a power, it makes it negative
$ rnd -n 2^8 -2^48
Number #1: -205186374661365.94
```

```
$ rnd -n 1E8
Number #1: 10408261.050660284
```