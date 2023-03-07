export const generatePassword = (length: number): string => {
    const lowercase = "abcdefghijklmnopqrstuvwxyz";
    const uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const digits = "0123456789";
    const symbols = "!@#$%^&*()_+~`|}{[]\:;?><,./-=";
    const charset = lowercase + uppercase + digits + symbols;

    let password = "";
    let hasLowercase = false;
    let hasUppercase = false;
    let hasDigit = false;
    let hasSymbol = false;

    // Add at least one character of each type
    password += lowercase[Math.floor(Math.random() * lowercase.length)];
    password += uppercase[Math.floor(Math.random() * uppercase.length)];
    password += digits[Math.floor(Math.random() * digits.length)];
    password += symbols[Math.floor(Math.random() * symbols.length)];

    for (let i = 4; i < length; i++) {
        const randomIndex = Math.floor(Math.random() * charset.length);
        const randomChar = charset[randomIndex];
        password += randomChar;

        // Keep track of which character types are present
        hasLowercase = hasLowercase || lowercase.includes(randomChar);
        hasUppercase = hasUppercase || uppercase.includes(randomChar);
        hasDigit = hasDigit || digits.includes(randomChar);
        hasSymbol = hasSymbol || symbols.includes(randomChar);
    }

    // If password is missing a required character type, add one more
    if (!hasLowercase) {
        const randomIndex = Math.floor(Math.random() * lowercase.length);
        password = password.substring(0, randomIndex) + lowercase[randomIndex] + password.substring(randomIndex + 1);
    }
    if (!hasUppercase) {
        const randomIndex = Math.floor(Math.random() * uppercase.length);
        password = password.substring(0, randomIndex) + uppercase[randomIndex] + password.substring(randomIndex + 1);
    }
    if (!hasDigit) {
        const randomIndex = Math.floor(Math.random() * digits.length);
        password = password.substring(0, randomIndex) + digits[randomIndex] + password.substring(randomIndex + 1);
    }
    if (!hasSymbol) {
        const randomIndex = Math.floor(Math.random() * symbols.length);
        password = password.substring(0, randomIndex) + symbols[randomIndex] + password.substring(randomIndex + 1);
    }

    return password;
}