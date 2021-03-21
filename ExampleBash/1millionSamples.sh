# Useufll for testing randomnes above 80k samples
# At 1 million samples produces around ~18 MB of data

args="-n -c -l"

# C-like for loops are insanley faster than making a big ol list
for ((n=0; n<50000; n++))
do
    # Bash has += for strings, look at that!!!
    args+=' -1E8 1E8'
done

echo "Samples" > test_output.csv

for ((n=0; n<20; n++))
do
    dotnet run -v q -p ../ -- $args >> test_output.csv
done


exit 0
