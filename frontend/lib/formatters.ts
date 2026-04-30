/**
 * Formats a number by rounding down to the nearest ten and adding a "+" suffix.
 * Example: 32 -> "30+", 37 -> "30+", 40 -> "40+"
 * 
 * @param value The numeric value to format
 * @returns The formatted string
 */
export function formatStatValue(value: number | undefined): string {
  if (value === undefined || value === null) return "...";
  
  // Logic: round down to the nearest 10
  // If value is 7, it becomes 0+. If we want it to be at least the number if it's < 10, 
  // we could change the logic, but the user said "cada decena", so 0-9 is 0, 10-19 is 10, etc.
  // Actually, if I have 7 battles, showing 0+ might be weird. 
  // But the user said: "si tengo 32, 37, 38 es lo mismo, deberia reflejar +30, recien se actualiza cuando lleggue a 40"
  // So the rule is floor(n/10)*10.
  
  const rounded = Math.floor(value / 10) * 10;
  return `${rounded}+`;
}
