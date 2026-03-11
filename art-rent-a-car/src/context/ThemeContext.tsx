import { createContext, useMemo, useState, ReactNode, useEffect } from "react";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";

interface ThemeContextType {
  toggleTheme: () => void;
  mode: "light" | "dark";
}

export const ThemeContext = createContext<ThemeContextType>({
  toggleTheme: () => {},
  mode: "dark",
});

export const CustomThemeProvider = ({ children }: { children: ReactNode }) => {
  const [mode, setMode] = useState<"light" | "dark">("dark");

  // Load saved theme
  useEffect(() => {
    const saved = localStorage.getItem("themeMode") as "light" | "dark" | null;
    if (saved) setMode(saved);
  }, []);

  const toggleTheme = () => {
    const newMode = mode === "dark" ? "light" : "dark";
    setMode(newMode);
    localStorage.setItem("themeMode", newMode);
  };

  const theme = useMemo(
    () =>
      createTheme({
        palette: {
          mode,
          primary: {
            main: mode === "dark" ? "#00e5ff" : "#111111",
          },
          background: {
            default: mode === "dark" ? "#0b0f19" : "#f5f7fa",
            paper:
              mode === "dark"
                ? "rgba(255, 255, 255, 0.06)"
                : "rgba(255, 255, 255, 0.8)",
          },
        },
        shape: {
          borderRadius: 20,
        },
        typography: {
          fontFamily: "Inter, sans-serif",
        },
        components: {
          MuiPaper: {
            styleOverrides: {
              root: {
                backdropFilter: "blur(12px)",
                WebkitBackdropFilter: "blur(12px)",
                transition: "all 0.3s ease",
              },
            },
          },
          MuiButton: {
            styleOverrides: {
              root: {
                borderRadius: 14,
                textTransform: "none",
                fontWeight: 600,
              },
            },
          },
        },
      }),
    [mode],
  );

  return (
    <ThemeContext.Provider value={{ toggleTheme, mode }}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        {children}
      </ThemeProvider>
    </ThemeContext.Provider>
  );
};
