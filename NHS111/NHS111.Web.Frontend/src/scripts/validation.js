module.exports =  {
  number: (input, min, max) => {
    // Strip all characters except numbers
    var result = input.replace(/[^0-9]/gi, '')

    // Ensure the remaining result is a valid number
    result = parseInt(result)

    // Make it negative if required
    if (input[0] == "-") result = -result

    // Ensure the number is within given bounds
    if (min && input < min) result = min
    if (max && input > max) result = max

    return result
  },
  date: (year, month, day) => {
    day = parseInt(day)
    month = parseInt(month-1)
    year = parseInt(year)

    const date = new Date(year, month, day)

    if (date.getFullYear() === year &&
        date.getMonth() === month &&
        date.getDate() === day) {
      return date
    }
  }
}
