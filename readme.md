# Random String Generator

This program is intended as a command line utility. It has a few quirky features and could maybe be useful to you if you have a unix or unix-like os.

Disclaimer: This was a really quick project that took like 20 minuets, I probably won't update it.

## Arguments and Usage

```
rnd [-h] [-t] [-c] len [len2...]
Prints a random sequence of characters of length len
  -t prints the time taken in seconds
  -c prints just the string of characters and nothing else, usefull if using in a bash script
  -h The algorithm only selects only characters from the alphabet and latin numerical characters (1,2,3,4 etc)
```

You can generate many different passwords by giving multiple lengths. They will all use different secure-random seeds.

### Example Usage

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
$ rnd -c -h -t 15
0.0036382
Zd7i8ASbrHRr85U
```

```
$ rnd 3 10 -c 5 3
Jc9,x2vwbno6aV,dpRfo,4jb
```

```
$ rnd 3 10 -c 5 -t 3
0.0036441
8L3,aowD2UQKbr,3mokA,5WE
```

## Multi-threaded

Yes, you read that correctly. There really is no point to this because to generate intMax characters would only take approx 9.21 seconds on my computer (then like 10 years to print those characters to the console lol). Any strings less than 15000 characters will use single-threaded mode.

I only did this because I wanted to refresh my knowledge of how to do that stuff.
