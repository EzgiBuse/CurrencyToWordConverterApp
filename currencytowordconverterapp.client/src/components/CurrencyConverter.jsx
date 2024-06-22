import React, { useState } from 'react';

function CurrencyConverter() {
    const [amount, setAmount] = useState('');
    const [result, setResult] = useState('');
    const [error, setError] = useState('');

    const handleConvert = async () => {
        setError('');
        setResult('');

        if (!amount) {
            setError('Amount cannot be empty.');
            return;
        }

        // Client-side validation for amount format
        const amountPattern = /^[0-9]+(,[0-9]{1,2})?$/;
        if (!amountPattern.test(amount)) {
            setError('Invalid amount format. Correct amount should be between 0 - 999999999 and "," separator should be use for cents(0 - 99)');
            return;
        }

        // Connecting to the server for the conversion
        try {
           
            const response = await fetch(`https://localhost:7094/api/Conversion/${amount}`);
            
            if (!response.ok) {
                const errorText = await response.text();
                setError(`Error: ${errorText}`);
                return;
            }
            const data = await response.text();
           
            setResult(data);
        } catch (error) {
          
            setError('Error converting amount. Please try again.');
        }
    };

    return (
        <div>
            <h1>Currency Converter</h1>
            <input
                type="text"
                value={amount}
                onChange={(e) => setAmount(e.target.value)}
                placeholder="Enter amount (e.g., 12345,67)"
            />
            <button onClick={handleConvert}>Convert</button>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {result && <p style={{ color: 'green' }}>{result}</p>}
        </div>
    );
}

export default CurrencyConverter;