const path = require('path');

module.exports = {
    entry: './src/index.js', // Use o caminho correto do seu arquivo de entrada
    output: {
        path: path.resolve(__dirname, 'wwwroot/js'),
        filename: 'index.js',
    },
    resolve: {
        extensions: ['.js', '.jsx', '.ts', '.tsx'],
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx|ts|tsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-env', '@babel/preset-react']
                    }
                }
            }
        ]
    },
    mode: 'development',
};
