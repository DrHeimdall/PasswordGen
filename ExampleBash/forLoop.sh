# Useufll for testing randomnes

args="-n -c -l"

# C-like for loops are insanley faster than making a big ol list
for ((n=0; n<83000; n++))
do
    # Bash has += for strings, look at that!!!
    args+=' -2^8 2^8'
done

echo "Samples" > test_output.csv

dotnet run -v q -p ../ -- $args >> test_output.csv

exit 0
